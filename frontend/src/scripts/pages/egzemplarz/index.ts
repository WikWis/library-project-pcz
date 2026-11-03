import {PieceService, type PieceServiceInterface} from "../../../services/piece-service.ts";
import type {PieceDTO} from "../../../dto/piece/piece-dto.ts";
import type {PieceLookupDTO} from "../../../dto/piece/piece-lookup-dto.ts";
import {renderPieceTable} from "../../renderers/piece/piece-table-render.ts";
import type {PageDataProviderInterface} from "../../pagination/page-data-provider/page-data-provider.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import {setupPagination} from "../../pagination/pagination.ts";

const pieceService: PieceServiceInterface = new PieceService()

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const pieceTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#pieces-table")

const pageDataProvider: PageDataProviderInterface<PieceDTO> = new class implements PageDataProviderInterface<PieceDTO> {
    pieceService: PieceServiceInterface = new PieceService()

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.pieceService.Index(undefined, pageNumber, 10)).length;
    }

    async providePage(pageNumber: Number): Promise<PieceDTO[]> {
        const pieceLookupDTOS: PieceLookupDTO[] = await pieceService.Index(undefined, pageNumber, 10);

        const result: (PieceDTO | undefined)[] = await Promise.all(
            pieceLookupDTOS.map((piece) => pieceService.Details(piece.pieceId))
        )

        return result.filter((item): item is PieceDTO => item !== undefined);
    }
}
const pageRenderer: PageRendererInterface<PieceDTO> = new class implements PageRendererInterface<PieceDTO> {
    render(data: PieceDTO[]): string {
        return renderPieceTable(data);
    }
}

export async function init() {
    if (pieceTableDiv === null || previousButton === null || nextButton === null) {
        throw new Error("No all elements needed are present.");
    }

    const pageSwitcher: PageSwitcher<PieceDTO> = new PageSwitcher<PieceDTO>(pageDataProvider, pageRenderer, pieceTableDiv)

    await setupPagination(pageSwitcher, previousButton, nextButton);
}