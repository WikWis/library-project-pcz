import type {PieceLookupDTO} from "../dto/piece/piece-lookup-dto.ts";
import {config} from "../config/config.ts";
import axios, {type AxiosRequestConfig} from "axios";
import type {PieceDTO} from "../dto/piece/piece-dto.ts";
import type {UpdatePieceDTO} from "../dto/piece/update-piece-dto.ts";
import type {CreatePieceDTO} from "../dto/piece/create-piece-dto.ts";

export interface PieceServiceInterface {
    Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<PieceLookupDTO[]>

    Details(authorId: Number): Promise<PieceDTO | undefined>

    Create(titleId: Number, pieceDto: CreatePieceDTO): Promise<PieceDTO | undefined>

    Update(pieceId: Number, pieceDto: UpdatePieceDTO): Promise<PieceDTO | undefined>

    Delete(pieceId: Number): Promise<boolean>
}

export class PieceService implements PieceServiceInterface {

    private static endpoint: string = config.public.backendUrl + "/pieces"

    async Index(titleId?: Number, page?: Number, pageSize?: Number): Promise<PieceLookupDTO[]> {
        const config: AxiosRequestConfig = {
            params: {
                titleId: titleId ?? null,
                page: page ?? null,
                pageSize: pageSize ?? null
            }
        }

        const response = await axios.get<PieceLookupDTO[]>(PieceService.endpoint, config);
        return response.data;
    }

    async Details(pieceId: Number): Promise<PieceDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.get<PieceDTO>(
            PieceService.endpoint + `/${pieceId}`,
            config
        );

        if (response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Create(titleId: Number, pieceDto: CreatePieceDTO): Promise<PieceDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
            params: {
                titleId: titleId,
            }
        }

        const response = await axios.post<PieceDTO>(
            PieceService.endpoint,
            pieceDto,
            config
        );

        if (response.status == 400 || response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Update(pieceId: Number, pieceDto: UpdatePieceDTO): Promise<PieceDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.put<PieceDTO>(
            PieceService.endpoint + `/${pieceId}`,
            pieceDto,
            config
        );

        if (response.status == 404 || response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Delete(pieceId: Number): Promise<boolean> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.delete<PieceDTO>(
            PieceService.endpoint + `/${pieceId}`,
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