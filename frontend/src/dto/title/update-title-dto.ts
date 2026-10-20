import type {AuthorLookupDTO} from "../author/author-lookup-dto.ts";
import type {PublisherLookupDTO} from "../publisher/publisher-lookup-dto.ts";

export interface UpdateTitleDTO {
    TitleId: number;
    Name: string;
    DateOfPublishing: string;
    ISBN: number;
    Genres: string[];
    Authors: AuthorLookupDTO[];
    Publisher: PublisherLookupDTO | null;
}