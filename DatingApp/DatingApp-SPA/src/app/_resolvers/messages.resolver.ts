import { AuthService } from './../_services/auth.service';
import { Message } from './../_models/Message';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { PaginatedResult } from '../_models/Pagination';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(
        private userService: UserService,
        private authService: AuthService,
        private router: Router,
        private alertify: AlertifyService
    ){}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]>{
        return this.userService.getMessages(this.authService.decodedToken.nameid,this.pageNumber, this.pageSize,this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retriving Messages');
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}