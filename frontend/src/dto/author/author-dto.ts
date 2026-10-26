import type {TitleLookupDTO} from "../title/title-lookup-dto.ts";

export interface AuthorDTO {
    authorId: number;
    name: string;
    surname: string;
    titles: TitleLookupDTO[];
}