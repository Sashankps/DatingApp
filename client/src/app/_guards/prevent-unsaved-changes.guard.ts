import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditsComponent } from '../members/member-edits/member-edits.component';

@Injectable({
  providedIn: 'root',
})
export class PreventUnsavedChangesGuard
  implements CanDeactivate<MemberEditsComponent>
{
  canDeactivate(component: MemberEditsComponent): boolean {
    if (component.editForm?.dirty) {
      return confirm(
        'Are you sure to continue? All unsaved changes will be lost!'
      );
    }
    return true;
  }
}
