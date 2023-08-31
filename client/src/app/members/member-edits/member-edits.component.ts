import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-edits',
  templateUrl: './member-edits.component.html',
  styleUrls: ['./member-edits.component.css'],
})
export class MemberEditsComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.userAssignment();
    this.loadMember();
  }

  userAssignment() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => (this.user = user),
    });
  }

  loadMember() {
    if (!this.user) return;
    this.memberService
      .getMember(this.user.username)
      .pipe(take(1))
      .subscribe({
        next: (member) => (this.member = member),
      });
  }

  updateMember() {
    if (!this.user) return;
    this.memberService
      .updateMember(this.user.username, this.editForm?.value)
      .subscribe({
        next: () => {
          this.toastr.success('Succeded');
          this.editForm?.reset(this.member);
        },
      });
  }
}
