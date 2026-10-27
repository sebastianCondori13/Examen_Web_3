"""Correct two confirmed Git author records; default mode never publishes refs."""
import argparse
import json
import os
from pathlib import Path
import re
import subprocess

EXPECTED = {
    'main': '717645e3a6060fb1ca803788059888ba85888b0e',
    'dev': '518ba0c2011031f5161684c9b9b67909866fbad0',
    'feature/sebastian-api': '4c7d3a1a8063d9dbb7856afd5c36a583d8634452',
    'feature/adrian-web': 'e5ef3c3760bf6b7df8b938115d0d443e52ba8a91',
    'feature/facundo-mobile': 'efccc9b3cd860f114d8682a9a36e2d74e4dde939',
    'chore/integracion-pruebas': 'bcb7a7e7e2bcaa3a1771a3c5f22672bf7b3a54ea',
}
AUTHORS = {
    EXPECTED['feature/adrian-web']: ('AdrianSVJ', 'svj5002099@est.univalle.edu'),
    EXPECTED['feature/facundo-mobile']: ('facundorojasgastanaga-dev', 'facundorojasgastanaga@gmail.com'),
}


def git(*args, data=None):
    return subprocess.check_output(['git', *args], input=data)


def remote_refs(kind):
    return {ref: sha for line in git('ls-remote', kind, 'origin').decode().splitlines()
            for sha, ref in [line.split('\t')]}


def prepare():
    order = git('rev-list', '--reverse', '--topo-order', *sorted(set(EXPECTED.values()))).decode().splitlines()
    mapping = {}
    changed = []
    removed_signatures = 0
    for old in order:
        raw = git('cat-file', 'commit', old)
        header, message = raw.split(b'\n\n', 1)
        fields = []
        for line in header.split(b'\n'):
            if line.startswith(b' '):
                fields[-1] += b'\n' + line
            else:
                fields.append(line)
        modified = False
        updated = []
        for field in fields:
            if field.startswith(b'parent '):
                parent = field[7:].decode()
                replacement = mapping[parent]
                modified |= replacement != parent
                field = b'parent ' + replacement.encode()
            elif field.startswith(b'author ') and old in AUTHORS:
                match = re.fullmatch(rb'author .* <[^<>]+> (\d+ [+-]\d{4})', field)
                if not match:
                    raise RuntimeError('Unsupported author header: ' + old)
                name, email = AUTHORS[old]
                field = ('author ' + name + ' <' + email + '> ').encode() + match[1]
                modified = True
            updated.append(field)
        if modified:
            # An old cryptographic signature cannot validate a changed commit.
            removed_signatures += sum(x.startswith((b'gpgsig ', b'gpgsig-sha256 ')) for x in updated)
            updated = [x for x in updated if not x.startswith((b'gpgsig ', b'gpgsig-sha256 '))]
            corrected = b'\n'.join(updated) + b'\n\n' + message
            new = git('hash-object', '-t', 'commit', '-w', '--stdin', data=corrected).decode().strip()
            assert git('rev-parse', old + '^{tree}') == git('rev-parse', new + '^{tree}')
            # Keep the original committer, both dates, message and every file.
            for label in (b'committer ',):
                assert next(x for x in fields if x.startswith(label)) == next(x for x in updated if x.startswith(label))
            changed.append({'old': old, 'new': new})
        else:
            new = old
        mapping[old] = new
    heads = {name: mapping[sha] for name, sha in EXPECTED.items()}
    return {'expected': EXPECTED, 'heads': heads, 'changed_commits': changed,
            'removed_obsolete_signatures': removed_signatures,
            'author_corrections': [{'old': sha, 'new': mapping[sha], 'name': value[0], 'email': value[1]}
                                   for sha, value in AUTHORS.items()],
            'main_tree': git('rev-parse', heads['main'] + '^{tree}').decode().strip()}


def apply(report):
    current = remote_refs('--heads')
    if all(current.get('refs/heads/' + name) == sha for name, sha in report['heads'].items()):
        return 'Already applied; no duplicate operation.'
    for name, old in EXPECTED.items():
        if current.get('refs/heads/' + name) != old:
            raise RuntimeError('Remote changed; stop without overwriting: ' + name)
    prefix = 'refs/tags/respaldo-autorias-' + EXPECTED['main'][:12] + '/'
    tags = remote_refs('--tags')
    backup_specs = []
    for name, old in EXPECTED.items():
        destination = prefix + name
        if destination in tags and tags[destination] != old:
            raise RuntimeError('Existing backup differs: ' + destination)
        if destination not in tags:
            backup_specs.append(old + ':' + destination)
    if backup_specs:
        git('push', '--atomic', 'origin', *backup_specs)
    tags = remote_refs('--tags')
    assert all(tags.get(prefix + name) == old for name, old in EXPECTED.items())
    updates = {name: new for name, new in report['heads'].items() if new != EXPECTED[name]}
    leases = ['--force-with-lease=refs/heads/' + name + ':' + EXPECTED[name] for name in updates]
    specs = [new + ':refs/heads/' + name for name, new in updates.items()]
    # Atomic update or fail; leases reject concurrent edits. No plain --force.
    git('push', '--atomic', *leases, 'origin', *specs)
    current = remote_refs('--heads')
    assert all(current.get('refs/heads/' + name) == sha for name, sha in report['heads'].items())
    report['backup_prefix'] = prefix
    return 'Applied; original heads retained in backup tags.'


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--apply', action='store_true')
    args = parser.parse_args()
    report = prepare()
    report['result'] = apply(report) if args.apply else 'Preview only; no remote ref was changed.'
    print(json.dumps(report, indent=2, ensure_ascii=False))
    if os.environ.get('GITHUB_STEP_SUMMARY'):
        Path(os.environ['GITHUB_STEP_SUMMARY']).write_text('```json\n' + json.dumps(report, indent=2) + '\n```\n')


if __name__ == '__main__':
    main()
