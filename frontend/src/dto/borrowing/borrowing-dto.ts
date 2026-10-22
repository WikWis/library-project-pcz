import type {PieceLookupDTO} from "../piece/piece-lookup-dto.ts";


export interface BorrowingDTO {
    BorrowingId: number;
    StartDate: string;
    EndDate: string;
    Piece: PieceLookupDTO | null;
    UserId: number;
}