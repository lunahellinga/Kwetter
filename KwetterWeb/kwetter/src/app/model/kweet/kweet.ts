export class Kweet {
  id: number;
  kweetText: string;
  userName: string;
  createdAt: Date;
  tags: string[];
  mentionedUsers: string[];

  constructor(id: number, kweetText: string, userName: string, createdAt: Date, tags: string[] | null, mentionedUsers: string[] | null) {
    this.id = id;
    this.kweetText = kweetText;
    this.userName = userName;
    this.createdAt = createdAt;
    this.tags = tags || [];
    this.mentionedUsers = mentionedUsers || [];
  }
}
