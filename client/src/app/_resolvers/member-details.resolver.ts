import { Injectable } from '@angular/core';
import * as router from '@angular/router';
import { Observable, of } from 'rxjs';
import { MemberService } from '../_services/member.service';
import { Member } from '../_models/member';
import { Resolve } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class MemberDetailsResolver implements Resolve<Member> {
  constructor(private memberService: MemberService) {}
  resolve(route: router.ActivatedRouteSnapshot): Observable<Member> {
    return this.memberService.getMember(route.paramMap.get('username')!);
  }
}
