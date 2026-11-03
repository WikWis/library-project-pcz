import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import {renderTitleTable} from "../../renderers/titles/title-table-render.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import type {PageDataProviderInterface} from "../../pagination/page-data-provider.ts";


const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const publishersTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-table")

if (publishersTableDiv === null) {
    throw new Error("No publisher table!");
}

async function updateNavigationButtons(pageSwitcher: PageSwitcher<any>) {
    previousButton!.disabled = pageSwitcher.getCurrentPage().valueOf() === 0;
    nextButton!.disabled = (await pageSwitcher.getPageItemCount()).valueOf() < 10;
}

const pageRenderer: PageRendererInterface<TitleLookupDTO> = new class implements PageRendererInterface<TitleLookupDTO> {
    render(data: TitleLookupDTO[]): string {
        return renderTitleTable(data);
    }
}

const pageDataProvider: PageDataProviderInterface<TitleLookupDTO> = new class implements PageDataProviderInterface<TitleLookupDTO> {
    titleService: TitleServiceInterface = new TitleService()

    async providePage(pageNumber: Number): Promise<TitleLookupDTO[]> {
        return await this.titleService.Index(undefined, pageNumber, 10);
    }

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.titleService.Index(undefined, pageNumber, 10)).length;
    }
}

const pageSwitcher: PageSwitcher<TitleLookupDTO> = new PageSwitcher<TitleLookupDTO>(pageDataProvider, pageRenderer, publishersTableDiv)

export async function init() {

    await pageSwitcher.initialize()
    await updateNavigationButtons(pageSwitcher)

    previousButton!.onclick = () => {
        pageSwitcher.previousPage();
        updateNavigationButtons(pageSwitcher)
    }
    nextButton!.onclick = () => {
        pageSwitcher.nextPage();
        updateNavigationButtons(pageSwitcher)
    }
}