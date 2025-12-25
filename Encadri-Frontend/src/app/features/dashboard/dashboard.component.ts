import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { StatisticsService, DashboardStats } from '../../core/services/statistics.service';
import { UiCardComponent } from '../../shared/components/ui-card/ui-card.component';
import { UiButtonComponent } from '../../shared/components/ui-button/ui-button.component';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, UiCardComponent, UiButtonComponent, BaseChartDirective],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private statisticsService = inject(StatisticsService);

  stats = signal<DashboardStats | null>(null);
  loading = signal<boolean>(true);
  error = signal<string>('');

  // Chart configurations
  public pieChartType: ChartType = 'pie';
  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          padding: 15,
          font: {
            size: 12
          }
        }
      }
    }
  };

  public submissionChartData: ChartData<'pie'> = {
    labels: [],
    datasets: [{
      data: [],
      backgroundColor: [
        '#10b981', // Green - approved
        '#f59e0b', // Yellow - pending
        '#ef4444', // Red - rejected
        '#3b82f6', // Blue - reviewed
        '#f97316'  // Orange - needs revision
      ]
    }]
  };

  public lineChartType: ChartType = 'line';
  public lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: true,
        position: 'top'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        max: 20,
        ticks: {
          stepSize: 5
        }
      }
    }
  };

  public gradeTrendChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{
      label: 'Grade',
      data: [],
      borderColor: '#3b82f6',
      backgroundColor: 'rgba(59, 130, 246, 0.1)',
      fill: true,
      tension: 0.4
    }]
  };

  get user() {
    return this.authService.currentUser();
  }

  get isStudent() {
    return this.user?.userRole?.toLowerCase() === 'student';
  }

  get isSupervisor() {
    return this.user?.userRole?.toLowerCase() === 'supervisor';
  }

  ngOnInit() {
    this.loadDashboardStats();
  }

  loadDashboardStats() {
    this.loading.set(true);
    this.statisticsService.getDashboardStats().subscribe({
      next: (data) => {
        this.stats.set(data);
        this.updateCharts(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Failed to load dashboard statistics', err);
        this.error.set('Failed to load dashboard data');
        this.loading.set(false);
      }
    });
  }

  updateCharts(stats: DashboardStats) {
    // Update submission status pie chart
    const submissionLabels: string[] = [];
    const submissionData: number[] = [];

    if (stats.approvedSubmissions > 0) {
      submissionLabels.push('Approved');
      submissionData.push(stats.approvedSubmissions);
    }
    if (stats.pendingSubmissions > 0) {
      submissionLabels.push('Pending');
      submissionData.push(stats.pendingSubmissions);
    }
    if (stats.rejectedSubmissions > 0) {
      submissionLabels.push('Rejected');
      submissionData.push(stats.rejectedSubmissions);
    }
    if (stats.reviewedSubmissions && stats.reviewedSubmissions > 0) {
      submissionLabels.push('Reviewed');
      submissionData.push(stats.reviewedSubmissions);
    }
    if (stats.needsRevisionSubmissions && stats.needsRevisionSubmissions > 0) {
      submissionLabels.push('Needs Revision');
      submissionData.push(stats.needsRevisionSubmissions);
    }

    this.submissionChartData = {
      labels: submissionLabels,
      datasets: [{
        data: submissionData,
        backgroundColor: [
          '#10b981',
          '#f59e0b',
          '#ef4444',
          '#3b82f6',
          '#f97316'
        ]
      }]
    };

    // Update grade trend chart for students
    if (this.isStudent && stats.gradeTrend && stats.gradeTrend.length > 0) {
      this.gradeTrendChartData = {
        labels: stats.gradeTrend.map((item: any) => {
          const title = item.title || 'Submission';
          return title.length > 15 ? title.substring(0, 15) + '...' : title;
        }),
        datasets: [{
          label: 'Grade (out of 20)',
          data: stats.gradeTrend.map((item: any) => item.grade || 0),
          borderColor: '#3b82f6',
          backgroundColor: 'rgba(59, 130, 246, 0.1)',
          fill: true,
          tension: 0.4
        }]
      };
    }
  }

  getProgressColor(percentage: number): string {
    if (percentage < 25) return '#ef4444'; // Red
    if (percentage < 50) return '#f59e0b'; // Orange
    if (percentage < 75) return '#3b82f6'; // Blue
    return '#10b981'; // Green
  }
}
