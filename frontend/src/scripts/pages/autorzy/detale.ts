import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {PageNumberProvider} from "../../pagination/page-number-provider.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {renderTitles} from "../../renderers/author/author-details-render.ts";

// Getting query param
const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

// For Not Found Validation
const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

// Astro elements
const authorIdDiv: HTMLDivElement | null  = document.querySelector<HTMLDivElement>("#author-id")
const authorNameDiv: HTMLDivElement | null  = document.querySelector<HTMLDivElement>("#author-name")
const authorSurnameDiv: HTMLDivElement | null  = document.querySelector<HTMLDivElement>("#author-surname")
const authorTitlesList: HTMLDivElement | null  = document.querySelector<HTMLDivElement>("#author-titles-list");
const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

// Services
const authorService: AuthorServiceInterface = new AuthorService();
const titlesService: TitleServiceInterface = new TitleService();

// Page Number Provider
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

async function changePage(pageNumber: Number, authorId: Number) {
    const titleListOnNextPage: TitleLookupDTO[] = await titlesService.Index(authorId, pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = titleListOnNextPage.length < 10;

    authorTitlesList!.innerHTML = renderTitles(titleListOnNextPage);
}

async function nextPage(authorId: Number) {
    await changePage(pageNumberProvider.provideNext(), authorId)
}

async function previousPage(authorId: Number) {
    await changePage(pageNumberProvider.providePrevious(), authorId)
}

function setUpAstroElements(authorDto: AuthorDTO) {
    authorIdDiv!.innerText = authorDto.authorId.toString()
    authorNameDiv!.innerText = authorDto.name;
    authorSurnameDiv!.innerText = authorDto.surname;
    authorSurnameDiv!.innerText = authorDto.surname;
}

export async function init() {
    const id: Number = Number.parseInt(pathId ?? "0")
    const authorDto: AuthorDTO | undefined = await authorService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || authorDto === undefined) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    setUpAstroElements(authorDto);
    await changePage(0, authorDto.authorId)

    previousButton!.onclick = () => {
        previousPage(authorDto.authorId)
    }
    nextButton!.onclick = () => {
        nextPage(authorDto.authorId)
    }
}