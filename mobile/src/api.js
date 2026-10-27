export const DEFAULT_URL=process.env.EXPO_PUBLIC_API_URL || 'http://10.0.2.2:5080/api';
export async function request(base,path,{token,method='GET',body}={}){
 const controller=new AbortController();const timer=setTimeout(()=>controller.abort(),12000);let r;
 try{r=await fetch(base.replace(/\/$/,'')+path,{method,signal:controller.signal,headers:{...(body?{'Content-Type':'application/json'}:{}),...(token?{Authorization:`Bearer ${token}`}:{})},body:body?JSON.stringify(body):undefined})}
 catch{throw new Error('No se pudo conectar. Revisa la IP del servidor, el WiFi y el firewall de Windows.')}finally{clearTimeout(timer)}
 if(r.status===204)return null;const data=await r.json().catch(()=>({}));
 if(!r.ok){const e=new Error(data.message||(data.errors?Object.values(data.errors).flat().join(' '):null)||({401:'Tu sesión terminó. Vuelve a iniciar sesión.',403:'Esta acción requiere Admin.',404:'El registro ya no existe.',429:'Espera un minuto antes de reintentar.'}[r.status])||'No se pudo completar la operación.');e.status=r.status;throw e}return data;
}
