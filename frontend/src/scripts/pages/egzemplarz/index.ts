import {PageNumberProvider} from "../../page-number-provider.ts";
import {PieceService, type PieceServiceInterface} from "../../../services/piece-service.ts";
import type {PieceDTO} from "../../../dto/piece/piece-dto.ts";
import type {PieceLookupDTO} from "../../../dto/piece/piece-lookup-dto.ts";
import {renderPieceTable} from "../../renderers/piece/piece-table-render.ts";

const pieceService: PieceServiceInterface = new PieceService()
const pageNumberProvider: PageNumberProvider = new PageNumberProvider();

const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")
const pieceTableDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#pieces-table")

async function getPieceDtoList(page: Number, pageSize: Number): Promise<PieceDTO[]> {
    const pieceLookupDTOS: PieceLookupDTO[] = await pieceService.Index(undefined, page, pageSize);

    const result: (PieceDTO | undefined)[] = await Promise.all(
        pieceLookupDTOS.map((piece) => pieceService.Details(piece.pieceId))
    )

    return result.filter((item): item is PieceDTO => item !== undefined);
}

async function changePage(pageNumber: Number) {
    const pieceListOnNextPage: PieceDTO[] = await getPieceDtoList(pageNumber, 10);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = pieceListOnNextPage.length < 10;

    pieceTableDiv!.innerHTML = renderPieceTable(pieceListOnNextPage);
}

async function nextPage() {
    await changePage(pageNumberProvider.provideNext())
}

async function previousPage() {
    await changePage(pageNumberProvider.providePrevious())
}

export async function init() {
    const initialPiecesList: PieceDTO[] = await getPieceDtoList(0, 10);

    pieceTableDiv!.innerHTML = renderPieceTable(initialPiecesList);

    previousButton!.disabled = pageNumberProvider.lookUp() === 0;
    nextButton!.disabled = initialPiecesList.length < 10;

    previousButton!.onclick = previousPage
    nextButton!.onclick = nextPage
}