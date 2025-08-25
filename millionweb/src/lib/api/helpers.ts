export async function safeJson<T>(res: Response): Promise<T> {
  const data = await res.json();
  return data as T;
}