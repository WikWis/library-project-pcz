import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import type {PageDataProviderInterface} from "../../pagination/page-data-provider/page-data-provider.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import {setupPagination} from "../../pagination/pagination.ts";
import {renderAuthorTable} from "../../renderers/author/author-table-render.ts";

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const authorsTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#authors-table")

const pageDataProvider: PageDataProviderInterface<AuthorLookupDTO> = new class implements PageDataProviderInterface<AuthorLookupDTO> {
    authorService: AuthorServiceInterface = new AuthorService()

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.authorService.Index(undefined, pageNumber, 10)).length;
    }

    async providePage(pageNumber: Number): Promise<AuthorLookupDTO[]> {
        return await this.authorService.Index(undefined, pageNumber, 10);
    }
}
const pageRenderer: PageRendererInterface<AuthorLookupDTO> = new class implements PageRendererInterface<AuthorLookupDTO> {
    render(data: AuthorLookupDTO[]): string {
        return renderAuthorTable(data);
    }
}

export async function init() {
    if (authorsTableDiv === null || previousButton === null || nextButton === null) {
        throw new Error("No all elements needed are present.");
    }

    const pageSwitcher: PageSwitcher<AuthorLookupDTO> = new PageSwitcher<AuthorLookupDTO>(pageDataProvider, pageRenderer, authorsTableDiv)

    await setupPagination(pageSwitcher, previousButton, nextButton);
}