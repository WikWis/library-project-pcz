import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";

export interface CreatePieceDTO {
    titleId: number;
    status: PieceStatus;
    state: PieceState;
}