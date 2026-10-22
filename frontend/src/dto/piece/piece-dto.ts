import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";
import type {BorrowingDTO} from "../borrowing/borrowing-dto.ts";


export interface PieceDTO {
    PieceId: number;
    TitleId: number;
    TitleName: string | null;
    Status: PieceStatus;
    State: PieceState;
    BorrowingHistory: BorrowingDTO[];
}