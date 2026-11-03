import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";

import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import type {PublisherDTO} from "../../../dto/publisher/publisher-dto.ts";

import type {PageDataProviderInterface} from "../../pagination/page-data-provider/page-data-provider.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import {setupPagination} from "../../pagination/pagination.ts";
import {renderTitlesGroupList} from "../../renderers/titles/title-group-list-render.ts";

const params = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

const publisherIdDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-id")
const publisherNameDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-name")
const publisherTitlesList = document.querySelector<HTMLDivElement>("#publisher-titles-list");

const previousButton = document.querySelector<HTMLButtonElement>("#previous-button");
const nextButton = document.querySelector<HTMLButtonElement>("#next-button");

const publisherService: PublisherServiceInterface = new PublisherService();

function setUpAstroElements(publisher: PublisherDTO) {
    publisherIdDiv!.innerText = publisher.publisherId.toString()
    publisherNameDiv!.innerText = publisher.name;
}

// For now renders every book, there is no endpoint for titles published by specific publisher
const pageDataProvider: PageDataProviderInterface<TitleLookupDTO> = new class implements PageDataProviderInterface<TitleLookupDTO> {
    titleService: TitleServiceInterface = new TitleService()

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.titleService.Index(undefined, pageNumber, 10)).length;
    }

    async providePage(pageNumber: Number): Promise<TitleLookupDTO[]> {
        return await this.titleService.Index(undefined, pageNumber, 10);
    }
}

const pageRenderer: PageRendererInterface<TitleLookupDTO> = new class implements PageRendererInterface<TitleLookupDTO> {
    render(data: TitleLookupDTO[]): string {
        return renderTitlesGroupList(data);
    }
}

export async function init(): Promise<void> {
    const id: Number = Number.parseInt(pathId ?? "0")
    const publisherDto: PublisherDTO | undefined = await publisherService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || publisherDto === undefined || publisherTitlesList === null || previousButton === null || nextButton === null) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    setUpAstroElements(publisherDto);

    const pageSwitcher: PageSwitcher<TitleLookupDTO> = new PageSwitcher<TitleLookupDTO>(pageDataProvider, pageRenderer, publisherTitlesList)
    await setupPagination(pageSwitcher, previousButton, nextButton);
}