"""Pruebas de integración contra una API local de desarrollo. Python 3 sin paquetes."""
import json,sys,uuid,urllib.request,urllib.error
BASE=(sys.argv[1] if len(sys.argv)>1 else 'http://localhost:5080').rstrip('/')
count=0
def call(path,method='GET',body=None,token=None,status=200):
 global count
 req=urllib.request.Request(BASE+path,data=json.dumps(body).encode() if body is not None else None,method=method,headers={**({'Content-Type':'application/json'} if body is not None else {}),**({'Authorization':'Bearer '+token} if token else {})})
 try:
  with urllib.request.urlopen(req,timeout=15) as r: code,raw=r.status,r.read()
 except urllib.error.HTTPError as e: code,raw=e.code,e.read()
 assert code==status,f'{method} {path}: esperado {status}, recibido {code}: {raw[:500]}'
 count+=1
 return json.loads(raw) if raw else None

def run():
 suffix=uuid.uuid4().hex[:10]
 call('/health')
 call('/api/products',status=401)
 call('/api/sales',status=401)
 call('/api/auth/login','POST',{'email':'admin@sporthub.com','password':'incorrecta'},status=401)
 admin=call('/api/auth/login','POST',{'email':'admin@sporthub.com','password':'SportHub2026!'})['token']
 assert call('/api/auth/me',token=admin)['role']=='Admin'
 call('/api/products',token=admin[:-6]+'xxxxxx',status=401)
 reg={'name':'Prueba integración','email':suffix+'@example.com','password':'Prueba2026!'}
 seller=call('/api/auth/register','POST',reg,status=201)['token']
 call('/api/auth/register','POST',reg,status=409)
 call('/api/auth/register','POST',{**reg,'email':'otro'+suffix+'@example.com','password':'123'},status=400)
 assert call('/api/auth/me',token=seller)['role']=='Vendedor'
 product={'name':'Producto '+suffix,'description':'Prueba temporal','price':25.5,'stock':10,'category':'Pruebas','brand':'QA','color':'#aabbcc'}
 call('/api/products','POST',product,seller,403)
 call('/api/products','POST',{**product,'price':-1},admin,400)
 call('/api/products','POST',{**product,'price':0},admin,400)
 cent=call('/api/products','POST',{**product,'name':'Centavo '+suffix,'price':0.01},admin,201)
 assert cent['price']==0.01
 call(f"/api/products/{cent['id']}",'DELETE',token=admin,status=204)
 call('/api/products','POST',{**product,'stock':-1},admin,400)
 call('/api/products','POST',{**product,'color':'red'},admin,400)
 p=call('/api/products','POST',product,admin,201);pid=p['id']
 assert call(f'/api/products/{pid}',token=admin)['stock']==10
 assert any(x['id']==pid for x in call('/api/products?category=Pruebas',token=admin))
 call(f'/api/products/{pid}','PUT',{**product,'name':'Actualizado '+suffix},admin)
 call('/api/sales','POST',{'customerName':'Prueba','notes':'','items':[]},seller,400)
 line={'productId':pid,'quantity':3}
 sale={'customerName':'Cliente QA','notes':'Prueba de stock','items':[line]}
 call('/api/sales','POST',{**sale,'items':[line,line]},seller,400)
 call('/api/sales','POST',{**sale,'items':[{**line,'quantity':-1}]},seller,400)
 call('/api/sales','POST',{**sale,'items':[{**line,'quantity':1000}]},seller,409)
 assert call(f'/api/products/{pid}',token=admin)['stock']==10
 v=call('/api/sales','POST',sale,seller,201);sid=v['id']
 assert v['total']==76.5
 assert call(f'/api/products/{pid}',token=admin)['stock']==7
 # Aislamiento entre cuentas incluso para Admin.
 call(f'/api/sales/{sid}',token=admin,status=404)
 call(f'/api/sales/{sid}','PUT',sale,admin,404)
 call(f'/api/sales/{sid}','DELETE',token=admin,status=404)
 call(f'/api/products/{pid}','DELETE',token=admin,status=409)
 call(f'/api/sales/{sid}','PUT',{**sale,'items':[{**line,'quantity':5}]},seller)
 assert call(f'/api/products/{pid}',token=admin)['stock']==5
 # Una edición fallida debe conservar tanto venta como stock.
 call(f'/api/sales/{sid}','PUT',{**sale,'customerName':'No persistir','items':[{**line,'quantity':999}]},seller,409)
 assert call(f'/api/products/{pid}',token=admin)['stock']==5
 v=call(f'/api/sales/{sid}',token=seller)
 assert v['customerName']=='Cliente QA' and v['total']==127.5
 # El precio guardado se conserva después de un cambio de catálogo.
 call(f'/api/products/{pid}','PUT',{**product,'price':40,'stock':5},admin)
 v=call(f'/api/sales/{sid}','PUT',sale,seller)
 assert v['total']==76.5
 assert call(f'/api/products/{pid}',token=admin)['stock']==7
 call(f'/api/sales/{sid}','DELETE',token=seller,status=204)
 assert call(f'/api/products/{pid}',token=admin)['stock']==10
 call(f'/api/products/{pid}','DELETE',token=admin,status=204)
 call(f'/api/products/{pid}',token=admin,status=404)
 print(f'PASS: {count} solicitudes verificadas; JWT, roles, CRUD, stock, rollback y aislamiento.')
if __name__=='__main__':run()
