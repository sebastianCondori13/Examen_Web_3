"""Compila y comprueba la API en una base temporal sin tocar los datos normales."""
from pathlib import Path
import os,subprocess,sys,tempfile,time,urllib.request,sqlite3
root=Path(__file__).resolve().parents[1]
project=root/'backend'/'SportHub.Api'
dotnet=os.environ.get('SPORTHUB_DOTNET','dotnet')
subprocess.run([dotnet,'build',str(project),'--nologo'],check=True)
with tempfile.TemporaryDirectory(prefix='sporthub-check-') as temporary:
 db=Path(temporary)/'test.db';logpath=Path(temporary)/'server.log'
 env={**os.environ,'ASPNETCORE_ENVIRONMENT':'Development','ASPNETCORE_URLS':'http://127.0.0.1:5089','ConnectionStrings__Default':f'Data Source={db};Foreign Keys=True'}
 # Ejecutar el DLL evita que launchSettings sustituya el puerto de pruebas.
 with logpath.open('w') as log:
  p=subprocess.Popen([dotnet,str(project/'bin/Debug/net10.0/SportHub.Api.dll')],cwd=project,env=env,stdout=log,stderr=log)
  try:
   for _ in range(120):
    if p.poll() is not None:raise RuntimeError('La API terminó antes de iniciar.')
    try:
     urllib.request.urlopen('http://127.0.0.1:5089/health',timeout=1);break
    except Exception:time.sleep(.25)
   else:raise RuntimeError('La API no respondió en el puerto 5089.')
   subprocess.run([sys.executable,str(root/'tests/api_smoke.py'),'http://127.0.0.1:5089'],check=True)
  except Exception:
   print(logpath.read_text(encoding='utf-8',errors='replace')[-8000:]);raise
  finally:
   p.terminate()
   try:p.wait(timeout=10)
   except subprocess.TimeoutExpired:p.kill();p.wait()
 with sqlite3.connect(db) as conn:
  tables={x[0] for x in conn.execute("select name from sqlite_master where type='table' and name not like 'sqlite_%'")}
  assert tables=={'Users','Products','Sales','SaleItems'},tables
  try:
   conn.execute("INSERT INTO Products (Name,Description,Price,Stock,Category,Brand,Color) VALUES ('Invalido','','0.0',1,'Prueba','QA','#aabbcc')")
   raise AssertionError('La base aceptó un precio cero.')
  except sqlite3.IntegrityError:
   conn.rollback()
 print('PASS: exactamente cuatro tablas de negocio y precio cero rechazado por SQLite. Base temporal eliminada al finalizar.')
