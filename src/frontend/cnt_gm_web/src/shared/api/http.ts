/**
 * Выполняет HTTP-запрос и возвращает тело ответа, распарсенное как JSON.
 *
 * @param url — URL (в приложении часто относительный путь с префиксом `/api`)
 * @param init — необязательные параметры `fetch`
 * @returns Распарсенное тело ответа типа `T`
 * @throws {Error} Если ответ не успешен (`ok === false`) или `fetch` выбросил исключение
 */
export async function httpJson<T>(url: string, init?: RequestInit): Promise<T> {
  const response = await fetch(url, init);
  if (!response.ok) {
    throw new Error(`HTTP ${response.status}`);
  }
  return response.json() as Promise<T>;
}
