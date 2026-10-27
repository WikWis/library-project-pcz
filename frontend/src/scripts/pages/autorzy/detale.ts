import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {PageNumberProvider} from "../../page-number-provider.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import {renderTitleTable} from "../../renderers/titles/title-table-render.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {renderAuthorDetails} from "../../renderers/author/author-details-render.ts";


const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

const authorDataDiv = document.querySelector<HTMLDivElement>("#author-data")
const titlesTable = document.querySelector<HTMLDivElement>("#titles-table")

const authorService: AuthorServiceInterface = new AuthorService();
const titlesService: TitleServiceInterface = new TitleService();
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

async function changePage(pageNumber: Number, authorId: Number) {
    const titleListOnNextPage: TitleLookupDTO[] = await titlesService.Index(authorId, pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = titleListOnNextPage.length < 10;

    titlesTable!.innerHTML = renderTitleTable(titleListOnNextPage);
}

async function nextPage(authorId: Number) {
    await changePage(pageNumberProvider.provideNext(), authorId)
}

async function previousPage(authorId: Number) {
    await changePage(pageNumberProvider.providePrevious(), authorId)
}

export async function init() {
    const id: Number = Number.parseInt(pathId ?? "0")
    const authorDto: AuthorDTO | undefined = await authorService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || authorDto === undefined) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    authorDataDiv!.innerHTML = renderAuthorDetails(authorDto);
    await changePage(0, authorDto.authorId)

    previousButton!.onclick = () => {
        previousPage(authorDto.authorId)
    }
    nextButton!.onclick = () => {
        nextPage(authorDto.authorId)
    }
}