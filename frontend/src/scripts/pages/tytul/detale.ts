import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import {TitleService, type TitleServiceInterface} from "../../../services/title-service.ts";
import {renderAuthors, renderGenres, renderPublisher} from "../../renderers/titles/title-details-render.ts";

const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

const titleIdDiv = document.querySelector<HTMLDivElement>("#title-id")
const titleNameDiv = document.querySelector<HTMLDivElement>("#title-name")
const titlePublishingDateDiv = document.querySelector<HTMLDivElement>("#title-publishing-date")
const titleIsbnDiv = document.querySelector<HTMLDivElement>("#title-isbn")
const titleGenresDiv = document.querySelector<HTMLDivElement>("#title-genres")
const titleAuthorsDiv = document.querySelector<HTMLDivElement>("#title-authors")
const titlePublisherDiv = document.querySelector<HTMLDivElement>("#title-publisher")

const titleService: TitleServiceInterface = new TitleService();

function setUpAstroElements(titleDto: TitleDTO) {
    titleIdDiv!.innerText = titleDto.titleId.toString()
    titleNameDiv!.innerText = titleDto.name
    titlePublishingDateDiv!.innerText = titleDto.dateOfPublishing
    titleIsbnDiv!.innerText = titleDto.isbn.toString()
    titleGenresDiv!.innerHTML = renderGenres(titleDto.genres)
    titleAuthorsDiv!.innerHTML = renderAuthors(titleDto.authors)
    titlePublisherDiv!.innerHTML = renderPublisher(titleDto.publisher)
}

export async function init() {
    const id: Number = Number.parseInt(pathId ?? "0")
    const titleDto: TitleDTO | undefined = await titleService.Details(id)

    if (id === 0 || isNaN(id.valueOf()) || titleDto === undefined) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    setUpAstroElements(titleDto)
}