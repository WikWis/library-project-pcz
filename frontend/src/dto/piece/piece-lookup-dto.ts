import type {PieceStatus} from "./piece-state.ts";
import type {PieceState} from "./piece-status.ts";


export interface PieceLookupDTO {
    PieceId: number;
    TitleId: number;
    Status: PieceStatus;
    State: PieceState;
}