export const API_URL = (import.meta.env.VITE_API_URL || 'http://localhost:5080/api').replace(/\/$/, '');
export async function request(path, {token, method='GET', body} = {}) {
  let response;
  try { response = await fetch(API_URL+path, {method, headers:{...(body?{'Content-Type':'application/json'}:{}), ...(token?{Authorization:`Bearer ${token}`}:{})}, body:body?JSON.stringify(body):undefined}); }
  catch { throw new Error('No se pudo conectar con la API. Comprueba que esté ejecutándose en el puerto 5080.'); }
  if(response.status===204) return null;
  const data=await response.json().catch(()=>({}));
  if(!response.ok){const error=new Error(data.message || (data.errors?Object.values(data.errors).flat().join(' '):null) || ({401:'Tu sesión terminó. Inicia sesión nuevamente.',403:'Esta acción requiere el rol Admin.',404:'El registro ya no existe.',429:'Demasiados intentos. Espera un minuto.'}[response.status]) || 'No se pudo completar la operación.'); error.status=response.status; throw error;}
  return data;
}
