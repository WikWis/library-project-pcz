import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";
import {renderPublisherTable} from "../../renderers/publisher/publisher-table-render.ts";
import type {PageDataProviderInterface} from "../../pagination/page-data-provider/page-data-provider.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import {setupPagination} from "../../pagination/pagination.ts";

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const publishersTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#publisher-table")

const pageDataProvider: PageDataProviderInterface<PublisherLookupDTO> = new class implements PageDataProviderInterface<PublisherLookupDTO> {
    publisherService: PublisherServiceInterface = new PublisherService()

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.publisherService.Index(pageNumber, 10)).length;
    }

    async providePage(pageNumber: Number): Promise<PublisherLookupDTO[]> {
        return await this.publisherService.Index(pageNumber, 10);
    }
}
const pageRenderer: PageRendererInterface<PublisherLookupDTO> = new class implements PageRendererInterface<PublisherLookupDTO> {
    render(data: PublisherLookupDTO[]): string {
        return renderPublisherTable(data);
    }
}

export async function init() {
    if (publishersTableDiv === null || previousButton === null || nextButton === null) {
        throw new Error("No all elements needed are present.");
    }

    const pageSwitcher: PageSwitcher<PublisherLookupDTO> = new PageSwitcher<PublisherLookupDTO>(pageDataProvider, pageRenderer, publishersTableDiv)

    await setupPagination(pageSwitcher, previousButton, nextButton);
}