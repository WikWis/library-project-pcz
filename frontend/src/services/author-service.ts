import type {AuthorLookupDTO} from "../dto/author/author-lookup-dto.ts";
import axios, {type AxiosRequestConfig} from "axios";
import {config} from "../config/config.ts";
import type {AuthorDTO} from "../dto/author/author-dto.ts";
import type {CreateAuthorDTO} from "../dto/author/create-author-dto.ts";
import type {UpdateAuthorDTO} from "../dto/author/update-author-dto.ts";

export interface AuthorServiceInterface {
    Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<AuthorLookupDTO[]>

    Details(authorId: Number): Promise<AuthorDTO | undefined>

    Create(authorDto: CreateAuthorDTO): Promise<AuthorDTO | undefined>

    Update(authorId: Number, authorDto: UpdateAuthorDTO): Promise<AuthorDTO | undefined>

    Delete(authorId: Number): Promise<boolean>
}

export class AuthorService implements AuthorServiceInterface {

    private static endpoint: string = config.public.backendUrl + "/authors"

    async Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<AuthorLookupDTO[]> {

        const config: AxiosRequestConfig = {
            params: {
                titleId: titleId ?? null,
                page: page ?? null,
                pageSize: pageSize ?? null
            }
        }

        const response = await axios.get<AuthorLookupDTO[]>(AuthorService.endpoint, config);
        return response.data;
    }

    async Details(authorId: Number): Promise<AuthorDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.get<AuthorDTO>(
            AuthorService.endpoint + `/${authorId}`,
            config
        );

        if (response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Create(authorDto: CreateAuthorDTO): Promise<AuthorDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.post<AuthorDTO>(
            AuthorService.endpoint,
            authorDto,
            config
        );

        if (response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Update(authorId: Number, authorDto: UpdateAuthorDTO): Promise<AuthorDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.put<AuthorDTO>(
            AuthorService.endpoint + `/${authorId}`,
            authorDto,
            config
        );

        if (response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Delete(authorId: Number): Promise<boolean> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.delete<AuthorDTO>(
            AuthorService.endpoint + `/${authorId}`,
            config
        );

        if (response.status == 204) {
            return true;
        }

        if (response.status == 404) {
            return false;
        }

        return false
    }

}