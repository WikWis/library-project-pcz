import { PublisherService, type PublisherServiceInterface } from "../../../services/publisher-service.ts";
import { TitleService, type TitleServiceInterface } from "../../../services/title-service.ts";
import { PageNumberProvider } from "../../page-number-provider.ts";

import type { TitleLookupDTO } from "../../../dto/title/title-lookup-dto.ts";
import type { PublisherDTO } from "../../../dto/publisher/publisher-dto.ts";

import {
    renderPublisherDetails,
    renderTitles
} from "../../renderers/publisher/publisher-details-render.ts";

const params = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

const publisherDataDiv = document.querySelector<HTMLDivElement>("#publisher-data")

const publisherService: PublisherServiceInterface = new PublisherService();
const titlesService: TitleServiceInterface = new TitleService();
const pageNumberProvider = new PageNumberProvider();

async function changePage(pageNumber: Number, publisherId: Number): Promise<void> {
    const titles: TitleLookupDTO[] = await titlesService.Index(publisherId, pageNumber, 10);

    const titlesList = document.querySelector<HTMLDivElement>("#titles-list");
    const previousButton = document.querySelector<HTMLButtonElement>("#previous-button");
    const nextButton = document.querySelector<HTMLButtonElement>("#next-button");


    titlesList!.innerHTML = renderTitles(titles);

    previousButton!.disabled = pageNumber === 0;
    nextButton!.disabled = titles.length < 10;
}

async function nextPage(authorId: Number) {
    await changePage(pageNumberProvider.provideNext(), authorId)
}

async function previousPage(authorId: Number) {
    await changePage(pageNumberProvider.providePrevious(), authorId)
}

export async function init(): Promise<void> {
    const id: Number = Number.parseInt(pathId ?? "0")
    const publisherDto: PublisherDTO | undefined = await publisherService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || publisherDto === undefined) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    publisherDataDiv!.innerHTML = renderPublisherDetails(publisherDto);
    await changePage(0, publisherDto.publisherId);

    const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
    const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

    previousButton!.onclick = () => {
        previousPage(publisherDto.publisherId)
    }
    nextButton!.onclick = () => {
        nextPage(publisherDto.publisherId)
    }
}