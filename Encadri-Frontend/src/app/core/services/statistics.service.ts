import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';

export interface DashboardStats {
  // Common stats
  totalProjects: number;
  activeProjects: number;
  completedProjects: number;

  totalSubmissions: number;
  pendingSubmissions: number;
  approvedSubmissions: number;
  rejectedSubmissions: number;

  totalMilestones: number;
  completedMilestones: number;

  totalMeetings: number;
  upcomingMeetings: number;

  // Student specific
  averageGrade?: number;
  needsRevisionSubmissions?: number;
  reviewedSubmissions?: number;
  inProgressMilestones?: number;
  pendingMilestones?: number;
  completedMeetings?: number;
  recentSubmissions?: any[];
  gradeTrend?: any[];

  // Supervisor specific
  totalStudents?: number;
  proposedProjects?: number;
  needsReviewSubmissions?: number;
  recentPendingSubmissions?: any[];
  projectProgress?: any[];
}

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private apiService = inject(ApiService);
  private authService = inject(AuthService);

  getDashboardStats(): Observable<DashboardStats> {
    const user = this.authService.currentUser();
    if (!user) {
      throw new Error('User not authenticated');
    }

    return this.apiService.get<DashboardStats>('/statistics', {
      userEmail: user.email,
      userRole: user.userRole
    });
  }
}
