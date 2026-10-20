import axios, {type AxiosRequestConfig} from "axios";
import {config} from "../config/config.ts";
import type {AuthorDTO} from "../dto/author/author-dto.ts";
import type {TitleLookupDTO} from "../dto/title/title-lookup-dto.ts";
import type {TitleDTO} from "../dto/title/title-dto.ts";
import type {CreateTitleDTO} from "../dto/title/create-title-dto.ts";
import type {UpdateTitleDTO} from "../dto/title/update-title-dto.ts";

export interface TitleServiceInterface {
    Index(authorId?: Number, page?: Number, pageSize?: Number): Promise<TitleLookupDTO[]>

    Details(titleId: Number): Promise<TitleDTO | undefined>

    Create(titleDto: CreateTitleDTO): Promise<TitleDTO | undefined>

    Update(titleId: Number, titleDto: UpdateTitleDTO): Promise<TitleDTO | undefined>

    Delete(titleId: Number): Promise<boolean>
}

export class TitleService implements TitleServiceInterface {

    private static endpoint: string = config.public.backendUrl + "/titles"

    async Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<TitleLookupDTO[]> {

        const config: AxiosRequestConfig = {
            params: {
                titleId: titleId ?? null,
                page: page ?? null,
                pageSize: pageSize ?? null
            }
        }

        const response = await axios.get<TitleLookupDTO[]>(TitleService.endpoint, config);
        return response.data;
    }

    async Details(titleId: Number): Promise<TitleDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.get<TitleDTO>(
            TitleService.endpoint + `/${titleId}`,
            config
        );

        if (response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Create(titleDto: CreateTitleDTO): Promise<TitleDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500
        }

        const response = await axios.post<TitleDTO>(
            TitleService.endpoint,
            titleDto,
            config
        );

        if (response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Update(titleId: Number, titleDto: UpdateTitleDTO): Promise<TitleDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.put<TitleDTO>(
            TitleService.endpoint + `/${titleId}`,
            titleDto,
            config
        );

        if (response.status == 404 || response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Delete(authorId: Number): Promise<boolean> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.delete<AuthorDTO>(
            TitleService.endpoint + `/${authorId}`,
            config
        );

        if (response.status == 204) {
            return true;
        }

        if (response.status == 404 || response.status == 400) {
            return false;
        }

        return false
    }

}