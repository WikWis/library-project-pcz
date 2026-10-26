import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";
import type {BorrowingDTO} from "../borrowing/borrowing-dto.ts";


export interface PieceDTO {
    pieceId: number;
    titleId: number;
    titleName: string | null;
    status: PieceStatus;
    state: PieceState;
    borrowingHistory: BorrowingDTO[];
}