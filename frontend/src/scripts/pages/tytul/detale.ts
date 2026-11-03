import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {renderTitleDetails} from "../../renderers/titles/title-details-render.ts";

const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")
const titleDataDiv = document.querySelector<HTMLDivElement>("#title-data")


const titleService: TitleServiceInterface = new TitleService();

export async function init() {
    const id: Number = Number.parseInt(pathId ?? "0")
    const titleDto: TitleDTO | undefined = await titleService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || titleDto === undefined) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    titleDataDiv!.innerHTML = renderTitleDetails(titleDto);
}