export interface PageDataProviderInterface<T> {
    providePage(pageNumber: Number): Promise<T[]>;

    getPageItemCount(pageNumber: Number): Promise<Number>;
}