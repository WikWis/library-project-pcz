import type {PageDataProviderInterface} from "./page-data-provider.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";

export class TitleLookupPageDataProvider implements PageDataProviderInterface<TitleLookupDTO> {
    private readonly titleService: TitleServiceInterface = new TitleService()
    private readonly authorId: Number;

    constructor(authorId: Number) {
        this.authorId = authorId;
    }


    async providePage(pageNumber: Number): Promise<TitleLookupDTO[]> {
        return await this.titleService.Index(this.authorId, pageNumber, 10);
    }

    async getPageItemCount(pageNumber: Number): Promise<Number> {
        return (await this.titleService.Index(this.authorId, pageNumber, 10)).length;
    }
}