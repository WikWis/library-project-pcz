import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";

export interface UpdatePieceDTO {
    pieceId: number;
    status: PieceStatus;
    state: PieceState;
}