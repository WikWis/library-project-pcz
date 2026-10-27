import type {AuthorDTO} from "../../../dto/author/author-dto.ts";


export function renderAuthorDetails(authorDto: AuthorDTO) {
    return `
        <h1>${authorDto.name} ${authorDto.surname}</h1>
    `
}