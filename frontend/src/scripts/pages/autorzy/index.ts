import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {PageNumberProvider} from "../../pagination/page-number-provider.ts";
import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import {renderAuthorTable} from "../../renderers/author/author-table-render.ts";


const authorService: AuthorServiceInterface = new AuthorService()
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const authorsTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#authors-table")

async function changePage(pageNumber: Number) {
    const authorListOnNextPage: AuthorLookupDTO[] = await authorService.Index(undefined, pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = authorListOnNextPage.length < 10;

    authorsTableDiv!.innerHTML = renderAuthorTable(authorListOnNextPage);
}

async function nextPage() {
    await changePage(pageNumberProvider.provideNext())
}

async function previousPage() {
    await changePage(pageNumberProvider.providePrevious())
}

export async function init() {
    await changePage(0)

    previousButton!.onclick = previousPage
    nextButton!.onclick = nextPage
}