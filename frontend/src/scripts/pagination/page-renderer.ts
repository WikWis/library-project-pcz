export interface PageRendererInterface<T> {
    render(data: T[]): string;
}