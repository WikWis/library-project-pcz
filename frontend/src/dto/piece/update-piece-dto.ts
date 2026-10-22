import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";

export interface UpdatePieceDTO {
    PieceId: number;
    Status: PieceStatus;
    State: PieceState;
}