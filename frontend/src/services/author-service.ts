import type {AuthorLookupDto} from "../dto/author/author-lookup-dto.ts";
import axios from "axios";
import {config} from "../config/config.ts";

export interface AuthorServiceInterface {
    Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<AuthorLookupDto[]>
}

export class AuthorService implements AuthorServiceInterface {

    private static endpoint: string = config.public.backendUrl + "/authors"

    async Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<AuthorLookupDto[]> {
        const response = await axios.get<AuthorLookupDto[]>(AuthorService.endpoint);
        return response.data;
    }

}