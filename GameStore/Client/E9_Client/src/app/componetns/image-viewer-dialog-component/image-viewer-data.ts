import { Image } from "src/app/models/image.model";

export interface ImageViewerData {
    activeIndex: number;
    showLarge: boolean;
    images: Image[];
}