import { Genre } from "../Genre";

export interface AddBook {
    title: string;
    description: string;
    imageLink?: string;
    author: string;
    genres: Genre[];
}