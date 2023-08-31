<div class="row" *ngIf="member">
    <div class="col-4">
        <h1>Your profile</h1>
    </div>
    <div class="col-8">
        <div class="alert alert-info pb-0" *ngIf="editForm.dirty">
            <p><strong>Information: </strong>You have made changes. Any unsaved changes will be lost</p>
        </div>
    </div>
    <div class="col-4">
        <div class="card">
            <img src="{{member.photoUrl || './assets/user.png'}}" alt="{{member.knownAs}}"
                class="card-img-top img-thumbnail">
            <div class="card-body">
                <div>
                    <strong>Location:</strong>
                    <p>{{member.city}}, {{member.country}}</p>
                </div>
                <div>
                    <strong>Age:</strong>
                    <p>{{member.age}}</p>
                </div>
                <div>
                    <strong>Last active:</strong>
                    <p>{{member.lastActive}}</p>
                </div>
                <div>
                    <strong>Member since:</strong>
                    <p>{{member.created}}</p>
                </div>
            </div>
            <div class="card-footer">
                <button
                    [disabled]="!editForm.dirty"
                    type="submit"
                    form="editForm"
                    class="btn btn-success col-12">Save Changes</button>
            </div>
        </div>
    </div>

    <div class="col-8">
        <tabset class="member-tabset">
            <tab heading="About {{member.knownAs}}">
                <form #editForm="ngForm" id="editForm" (ngSubmit)="updateMember()">
                    <h4 class="mt-2">Description</h4>
                    <textarea class="form-control" [(ngModel)]="member.introduction" name="introduction" rows="6"></textarea>
                    <h4 class="mt-2">Looking for</h4>
                    <textarea class="form-control" [(ngModel)]="member.lookingFor" name="lookingFor" rows="6"></textarea>
                    <h4>Interests</h4>
                    <textarea class="form-control" [(ngModel)]="member.interests" name="interests" rows="6"></textarea>
                    <h4 class="mt-2">Location Details: </h4>
                    <div class="d-flex flex-row align-items-center">
                        <label for="city">City: </label>
                        <input [(ngModel)]="member.city" type="text" name="city" class="form-control mx-2">
                        <label for="city">Country: </label>
                        <input [(ngModel)]="member.country" type="text" name="country" class="form-control mx-2">
                    </div>
                </form>

            </tab>
            <tab heading="Edit Photos">
                <p>Photo edit will go here</p>
            </tab>
        </tabset>
    </div>
</div>
