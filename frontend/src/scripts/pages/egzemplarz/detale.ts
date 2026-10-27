export function init() {
    const params: URLSearchParams = new URLSearchParams(window.location.search);
    const id: string | null = params.get("id");

    const textContent = document.querySelector<HTMLDivElement>("#id");

    textContent!.textContent = id ?? "0"
}