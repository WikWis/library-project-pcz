import {type PageNumberInterface, PageNumberProvider} from "./page-number-provider.ts";
import type {PageRendererInterface} from "./page-renderer.ts";
import type {PageDataProviderInterface} from "./page-data-provider/page-data-provider.ts";

export interface PageSwitcherInterface {
    nextPage(): void;

    previousPage(): void;
}

export class PageSwitcher<T> implements PageSwitcherInterface {

    private pageNumberProvider: PageNumberInterface;
    private pageRenderer: PageRendererInterface<T>;
    private pageContainer: Element;
    private pageDataProvider: PageDataProviderInterface<T>

    constructor(
        pageDataProvider: PageDataProviderInterface<T>,
        pageRenderer: PageRendererInterface<T>,
        pageContainer: Element,
    ) {
        this.pageNumberProvider = new PageNumberProvider();
        this.pageRenderer = pageRenderer;
        this.pageContainer = pageContainer;
        this.pageDataProvider = pageDataProvider
    }

    private async changePage(pageNumber: Number): Promise<void> {
        const data: T[] = await this.pageDataProvider.providePage(pageNumber);
        this.pageContainer.innerHTML = this.pageRenderer.render(data);
    }

    public async nextPage(): Promise<void> {
        const pageNumber = this.pageNumberProvider.provideNext();
        await this.changePage(pageNumber);
    }

    public async previousPage(): Promise<void> {
        const pageNumber = this.pageNumberProvider.providePrevious();
        await this.changePage(pageNumber);
    }

    public getCurrentPage(): Number {
        return this.pageNumberProvider.lookUp();
    }

    async getPageItemCount(): Promise<Number> {
        return await this.pageDataProvider.getPageItemCount(this.pageNumberProvider.lookUp());
    }

    public async initialize(): Promise<void> {
        await this.changePage(this.pageNumberProvider.lookUp());
    }
}