import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class PagerService {
    getPager(totalItems: number, currentPage: number = 1, pageSize: number = 10) {
        // Calcular total de páginas
        let totalPages = Math.ceil(totalItems / pageSize);

        //Garantir que a página atual não está fora do limite
        if (currentPage < 1) {
            currentPage = 1;
        } else if (currentPage > totalPages) {
            currentPage = totalPages;
        }

        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            //Menos de 10 páginas, exibir todas.
            startPage = 1;
            endPage = totalPages;
        } else {
            //Mais de 10 páginas, calcular início e fim da exibição.
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        //Calcular índices de início e fim.
        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        //Criar o array de páginas para o ng-repeat do paginador.
        let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);

        //Retornar o objeto com o paginador para a view.
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
}
