import type {PieceLookupDTO} from "../piece/piece-lookup-dto.ts";


export interface BorrowingDTO {
    borrowingId: number;
    startDate: string;
    endDate: string;
    piece: PieceLookupDTO | null;
    userId: number;
}