export class Claims {
    public developer: string = 'Developer'
    public manageUsers: string = 'ManageUsers'
    public users: { view: string } = {
        view: 'ManageUsers.Users.View'
    }
    public roles: { view: string } = {
        view: 'ManageUsers.Roles.View'
    }
}