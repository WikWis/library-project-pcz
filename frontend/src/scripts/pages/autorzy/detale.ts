import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {PageNumberProvider} from "../../page-number-provider.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {renderAuthorDetails, renderTitles} from "../../renderers/author/author-details-render.ts";


const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

const authorDataDiv = document.querySelector<HTMLDivElement>("#author-data")

const authorService: AuthorServiceInterface = new AuthorService();
const titlesService: TitleServiceInterface = new TitleService();
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

async function changePage(pageNumber: Number, authorId: Number) {
    const titlesList = document.querySelector<HTMLDivElement>("#titles-list");
    const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
    const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

    const titleListOnNextPage: TitleLookupDTO[] = await titlesService.Index(authorId, pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = titleListOnNextPage.length < 10;

    titlesList!.innerHTML = renderTitles(titleListOnNextPage);
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

    const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
    const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

    previousButton!.onclick = () => {
        previousPage(authorDto.authorId)
    }
    nextButton!.onclick = () => {
        nextPage(authorDto.authorId)
    }
}