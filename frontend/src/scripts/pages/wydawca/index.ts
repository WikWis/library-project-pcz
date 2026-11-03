import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import {PageNumberProvider} from "../../pagination/page-number-provider.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";
import {renderPublisherTable} from "../../renderers/publisher/publisher-table-render.ts";


const publisher: PublisherServiceInterface = new PublisherService()
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const publishersTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-table")

async function changePage(pageNumber: Number) {
    const publisherListOnNextPage: PublisherLookupDTO[] = await publisher.Index(pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = publisherListOnNextPage.length < 10;

    publishersTableDiv!.innerHTML = renderPublisherTable(publisherListOnNextPage);
}

async function nextPage() {
    await changePage(pageNumberProvider.provideNext())
}

async function previousPage() {
    await changePage(pageNumberProvider.providePrevious())
}

export async function init() {
    const initialPublishersList: PublisherLookupDTO[] = await publisher.Index(0, 10);

    publishersTableDiv!.innerHTML = renderPublisherTable(initialPublishersList);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = initialPublishersList.length < 10;

    previousButton!.onclick = previousPage
    nextButton!.onclick = nextPage
}