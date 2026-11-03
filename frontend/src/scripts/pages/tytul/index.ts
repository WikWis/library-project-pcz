import {PageNumberProvider} from "../../page-number-provider.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import {renderTitleTable} from "../../renderers/titles/title-table-render.ts";


const title: TitleServiceInterface = new TitleService()
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const publishersTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-table")

async function changePage(pageNumber: Number) {
    const titleListOnNextPage: TitleLookupDTO[] = await title.Index(undefined, pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = titleListOnNextPage.length < 10;

    publishersTableDiv!.innerHTML = renderTitleTable(titleListOnNextPage);
}

async function nextPage() {
    await changePage(pageNumberProvider.provideNext())
}

async function previousPage() {
    await changePage(pageNumberProvider.providePrevious())
}

export async function init() {
    const initialTitleList: TitleLookupDTO[] = await title.Index(undefined, 0, 10);

    publishersTableDiv!.innerHTML = renderTitleTable(initialTitleList);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = initialTitleList.length < 10;

    previousButton!.onclick = previousPage
    nextButton!.onclick = nextPage
}