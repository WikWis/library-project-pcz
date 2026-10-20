import type {TitleLookupDTO} from "../title/title-lookup-dto.ts";


export interface AuthorDTO {
    AuthorId: number;
    Name: string;
    Surname: string;
    Titles: TitleLookupDTO[];
}